import { Injectable, Component, OnChanges, Input, ViewChild } from '@angular/core';
import { PackageDTO } from 'src/app/sources/sources.model';
import { DownloadDialogComponent } from '../download-dialog/download-dialog.component';


@Component({
  selector: 'app-package-info',
  templateUrl: './package-info.component.html'
})

@Injectable()
export class PackageInfoComponent {
  public showIcon = false;

  @ViewChild(DownloadDialogComponent) downloadDialog: DownloadDialogComponent;
  showDownloadDialogModal() {
    this.downloadDialog.showModal();
  }

  @Input()
  public package: PackageDTO;

  constructor() {
  }

  init() {
  }


}
