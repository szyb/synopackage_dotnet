import { Component, ViewChild, Injectable, Input } from '@angular/core';
import { ModalDirective } from 'angular-bootstrap-md';


@Component({
  selector: 'app-download-dialog',
  templateUrl: './download-dialog.component.html'
})

@Injectable()
export class DownloadDialogComponent {

  @Input()
  public link: string;

  @Input()
  public packageName: string;

  @Input()
  public packageIconLocation: string;

  @Input()
  public packageIsBeta: boolean;

  @Input()
  public packageVersion: string;

  @Input()
  public packageDescription: string;


  constructor() {
  }

  @ViewChild(ModalDirective) public downloadModal: ModalDirective;

  showModal = () => {
    this.downloadModal.show();
  }

  download() {
    window.location.href = this.link;
    this.downloadModal.hide();
  }

}
