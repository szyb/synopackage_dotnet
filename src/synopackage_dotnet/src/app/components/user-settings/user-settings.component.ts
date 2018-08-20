import { Component, ViewChild, Injectable } from '@angular/core';
import { ModalDirective } from 'angular-bootstrap-md';
import { UserSettingsService } from './user-settings.service';
import { Config } from '../../shared/config';
import { HttpClient } from '@angular/common/http';





@Component({
  selector: 'app-user-settings',
  templateUrl: './user-settings.component.html',
  styleUrls: ['./user-settings.component.scss']
})

@Injectable()
export class UserSettingsComponent {
  private versions: VersionDTO[];
  private selectedVersion: string;
  private selectedModel: string;

  constructor(http: HttpClient, private userSettingsService: UserSettingsService) {
        http.get<VersionDTO[]>(`${Config.apiUrl}Versions/GetAll`).subscribe( result => {
          this.versions = result;
        });
        this.selectedVersion = this.userSettingsService.getUserVersion();
        this.selectedModel = this.userSettingsService.getUserModel();
  }

  @ViewChild(ModalDirective) public basicModal: ModalDirective;

  showModal = () => {
    this.basicModal.show();
  }

  save() {
    this.validateVersion(this.selectedVersion);
    this.userSettingsService.saveUserSettings(this.selectedVersion, 'DS718+');
    this.basicModal.hide();
  }

  validateVersion(version: string): void {
    const ver = this.versions.find(x => x.version === version);
    if (ver === null) {
      throw new Error('Selected version is invalid');
    }
  }
}



interface VersionDTO {
    version: string;
  }
