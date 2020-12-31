import { Component, ViewChild, Injectable, Input } from '@angular/core';
import { ModalDirective } from 'angular-bootstrap-md';
import { SourcesService } from 'src/app/shared/sources.service';
import { PackageDTO } from 'src/app/sources/sources.model';
import { take } from 'rxjs/operators';
import { DownloadSpkService } from 'src/app/shared/download-spk.service';
import { environment } from 'src/environments/environment';


@Component({
  selector: 'app-download-dialog',
  templateUrl: './download-dialog.component.html'
})

@Injectable()
export class DownloadDialogComponent {
  public dialogShown = false;
  public waitForDownload = false;

  @Input()
  public package: PackageDTO;

  constructor(private downloadSpkService: DownloadSpkService) {
  }

  @ViewChild(ModalDirective, { static: true }) public downloadModal: ModalDirective;

  showModal = () => {
    this.waitForDownload = false;
    this.dialogShown = true;
    this.downloadModal.show();
  }

  download() {
    if (this.waitForDownload === true)
      return;
    this.waitForDownload = true;
    const result = this.downloadSpkService.downloadRequest(this.package.downloadLink, this.package.sourceName, this.package.name);
    result.pipe(
      take(1)
    ).subscribe(item => {
      if (item) {
        if (item.startsWith("http"))
          document.location.href = item;
        else {
          document.location.href = environment.restBaseUrl.concat(item);
        }
        // if (item.startsWith("http")) { //external link
        //   window.open(item, "_blank", "noopener noreferrer");
        // }
        // else { //proxy download
        //   window.open(environment.restBaseUrl.concat(item), "_blank", "noopener noreferrer");
        // }
      } else {
        alert('The package could not be downloaded');
        this.waitForDownload = false;
      }
      this.downloadModal.hide();
    });

  }

}
