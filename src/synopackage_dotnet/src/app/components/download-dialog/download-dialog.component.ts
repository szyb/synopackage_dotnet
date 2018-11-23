import { Component, ViewChild, Injectable, Input } from '@angular/core';
import { ModalDirective } from 'angular-bootstrap-md';
import { SourcesService } from 'src/app/shared/sources.service';
import { PackageDTO } from 'src/app/sources/sources.model';


@Component({
  selector: 'app-download-dialog',
  templateUrl: './download-dialog.component.html'
})

@Injectable()
export class DownloadDialogComponent {
  public dialogShown = false;

  @Input()
  public package: PackageDTO;

  constructor(private sourcesService: SourcesService) {
  }

  @ViewChild(ModalDirective) public downloadModal: ModalDirective;

  showModal = () => {
    this.dialogShown = true;
    this.downloadModal.show();
  }

  download() {
    this.sourcesService.downloadRequest(this.package.downloadLink, this.package.sourceName, this.package.name);
    document.location.href = this.package.downloadLink;
    this.downloadModal.hide();
  }

}
