import { Component, ViewChild, Injectable, Input } from '@angular/core';
import { ModalDirective } from 'angular-bootstrap-md';
import { SourcesService } from 'src/app/shared/sources.service';
import { PackageDTO } from 'src/app/sources/sources.model';
import { take } from 'rxjs/operators';


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

  @ViewChild(ModalDirective, { static: true }) public downloadModal: ModalDirective;

  showModal = () => {
    this.dialogShown = true;
    this.downloadModal.show();
  }

  download() {
    const result = this.sourcesService.downloadRequest(this.package.downloadLink, this.package.sourceName, this.package.name);
    result.pipe(
      take(1)
    ).subscribe(item => {
      if (item) {
        document.location.href = this.package.downloadLink;
      } else {
        alert('The package could not be downloaded');
      }
      this.downloadModal.hide();
    });

  }

}
