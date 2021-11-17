import { Injectable, Component, OnChanges, Input, ViewChild, Output, EventEmitter, OnInit, AfterViewInit } from '@angular/core';
import { PaginatorDataProvider } from './paginator-data';
import { PageInfoDTO } from './paginator.model';


@Component({
  selector: 'app-paginator',
  styleUrls: ['./paginator.component.scss'],
  templateUrl: './paginator.component.html'
})

@Injectable()
export class PaginatorComponent implements OnInit {

  public info: PageInfoDTO;
  public shouldShowPaginator: boolean = false;
  constructor() {
    this.info = new PageInfoDTO();
  }

  private page: string;
  private totalPages: number;
  @Input() dataProvider: PaginatorDataProvider;
  @Output() changed = new EventEmitter<string>();

  ngOnInit() {

    this.dataProvider.dataChanged.subscribe(val => {
      this.page = val.currentPage;
      this.totalPages = val.totalPages;
      this.info.pages = this.getPages();
      this.page = val.currentPage;
      this.shouldShowPaginator = val.totalPages === 1 ? false : true;
    });
  }

  onClick(page: string) {
    if (page !== this.page) {
      this.shouldShowPaginator = false;
      this.changed.emit(page);
    }
  }

  onNext() {
    if (!this.isLast()) {
      this.shouldShowPaginator = false;
      this.changed.emit(this.nextPage());
    }
  }

  onPrev() {
    if (!this.isFirst()) {
      this.shouldShowPaginator = false;
      this.changed.emit(this.prevPage());
    }
  }

  isLast() {
    if (this.page == this.totalPages.toString())
      return true;
    else
      return false;
  }

  isFirst() {
    if (this.page == "1")
      return true;
    else
      return false;
  }

  nextPage() {
    if (!this.isLast())
      return (eval(this.page) + 1).toString();
    else
      return this.page;
  }

  prevPage() {
    if (!this.isFirst())
      return (eval(this.page) - 1).toString();
    else
      return this.page;
  }


  getPages(): string[] {
    var result: string[] = [];
    var lastPush: string;
    //logic:
    //0. always show current page
    //1. always show first and last page
    //2. if there is less than 6 pages then show all and ignore 3-5 rules
    //3. always show previuos and next page to the current
    //4. show first 3 pages if 1 is selected
    //5. show 3 last pages if last is selected
    for (var i = 1; i <= this.totalPages; i++) {
      if (this.shouldEmitPageNumber(i)) {
        result.push(i.toString());
        lastPush = i.toString();
      }
      else if (lastPush != "...") {
        result.push("...");
        lastPush = "...";
      }
    }
    return result;
  }

  shouldEmitPageNumber(i: number) {
    const pageInt = eval(this.page);
    if (i == pageInt) // rule 0
      return true;
    else if (i == 1 || i == this.totalPages) //rule 1
      return true;
    else if (this.totalPages < 6) //rule 2
      return true;
    else if (i == pageInt - 1 || i == pageInt + 1) //rule 3
      return true;
    else if (pageInt == 1 && i == 3) //rule 4
      return true;
    else if (pageInt == this.totalPages && i == this.totalPages - 2) //rule 5
      return true;
    else
      return false;
  }

  getItemClass(page: string): string {
    if (page == this.page)
      return "page-item active";
    else if (page == "-1" || page == "...")
      return "page-item disabled"
    else
      return "page-item";
  }

  getPrevClass(): string {
    if (this.page == "1")
      return "page-item disabled";
    else
      return "page-item"
  }
  getNextClass(): string {
    if (this.page == this.totalPages.toString())
      return "page-item disabled";
    else
      return "page-item"
  }

}
