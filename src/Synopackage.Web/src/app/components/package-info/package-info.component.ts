import { Injectable, Component, OnChanges, Input, ViewChild, OnInit } from '@angular/core';
import { PackageDTO } from 'src/app/sources/sources.model';
import { DownloadDialogComponent } from '../download-dialog/download-dialog.component';


@Component({
  selector: 'app-package-info',
  styleUrls: ['./package-info.component.scss'],
  templateUrl: './package-info.component.html'
})

@Injectable()
export class PackageInfoComponent implements OnInit {
  public defaultImage = 'assets/package.png';

  @Input() public package: PackageDTO;
  @Input() public downloadDialog: DownloadDialogComponent;
  @Input() public isDownloadDisabled: boolean;

  ngOnInit() {
    //console.log("init");
    //console.log(this.isDownloadDisabled);
  }

  showDownloadDialogModal() {
    //console.log(this.isDownloadDisabled);
    if (!this.isDownloadDisabled) {
      this.downloadDialog.package = this.package;
      this.downloadDialog.showModal();
    }
    else {
      alert('Download is disabled for this source server');
    }
  }

  getDownloadIconClass(): string {
    if (!this.isDownloadDisabled)
      return "green-text px-2 cursor-pointer";
    else
      return "grey-text px-2 disabled";
  }
}
