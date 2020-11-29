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

  private page: number;
  private totalPages: number;
  @Input() dataProvider: PaginatorDataProvider;
  @Output() changed = new EventEmitter<number>();

  ngOnInit() {

    this.dataProvider.dataChanged.subscribe(val => {
      this.page = val.currentPage;
      this.totalPages = val.totalPages;
      this.info.pages = this.getPages();
      this.page = val.currentPage;
      this.shouldShowPaginator = val.totalPages === 1 ? false : true;
    });
  }

  onClick(page: number) {
    if (page !== this.page) {
      this.shouldShowPaginator = false;
      this.changed.emit(page);
    }
  }

  onNext() {
    if (this.page < this.totalPages) {
      this.shouldShowPaginator = false;
      this.changed.emit(this.page + 1);
    }
  }

  onPrev() {
    if (this.page > 1) {
      this.shouldShowPaginator = false;
      this.changed.emit(this.page - 1);
    }
  }


  getPages(): number[] {
    var result: number[] = [];
    for (var i = 1; i <= this.totalPages; i++) {
      //TODO: generation logic
      result.push(i);
    }
    return result;
  }

  getItemClass(page: number): string {
    if (page === this.page)
      return "page-item active";
    else if (page === -1)
      return "page-item disabled"
    else
      return "page-item";
  }

  getPrevClass(): string {
    if (this.page === 1)
      return "page-item disabled";
    else
      return "page-item"
  }
  getNextClass(): string {
    if (this.page === this.totalPages)
      return "page-item disabled";
    else
      return "page-item"
  }

}
