import { Injectable, Component, OnChanges, Input, ViewChild } from '@angular/core';
import { PackageDTO } from 'src/app/sources/sources.model';
import { DownloadDialogComponent } from '../download-dialog/download-dialog.component';


@Component({
  selector: 'app-package-info',
  styleUrls: ['./package-info.component.scss'],
  templateUrl: './package-info.component.html'
})

@Injectable()
export class PackageInfoComponent {

  @Input()
  public package: PackageDTO;

  @ViewChild(DownloadDialogComponent) downloadDialog: DownloadDialogComponent;
  showDownloadDialogModal() {
    this.downloadDialog.showModal();
  }

  constructor() {
  }

  init() {
  }


}
