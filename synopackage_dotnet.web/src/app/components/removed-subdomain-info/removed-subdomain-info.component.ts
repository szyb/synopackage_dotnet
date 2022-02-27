import { Injectable, Component, OnChanges, Input, ViewChild, OnInit } from '@angular/core';
import { PackageDTO } from 'src/app/sources/sources.model';
import { DownloadDialogComponent } from '../download-dialog/download-dialog.component';


@Component({
  selector: 'app-removed-subdomain-info',
  styleUrls: ['./removed-subdomain-info.component.scss'],
  templateUrl: './removed-subdomain-info.component.html'
})

@Injectable()
export class RemovedSubdomainInfoComponent implements OnInit {
  public shouldShow: boolean;

  ngOnInit(): void {
    const untilDate = new Date(2022, 5, 31);
    const today = new Date();
    const isDateValid = untilDate > today;
    const removedSubDomainAck = localStorage.getItem("removedSubDomainAck");
    if (!removedSubDomainAck && isDateValid) {
      this.shouldShow = true;
    }
    else {
      this.shouldShow = false;
    }
  }

  public gotIt() {
    localStorage.setItem("removedSubDomainAck", "true");
    this.shouldShow = false;
  }


}
