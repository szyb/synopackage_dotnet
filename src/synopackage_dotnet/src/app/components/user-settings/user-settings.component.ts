import { Component, ViewChild, Injectable } from '@angular/core';
import { ModalDirective } from 'angular-bootstrap-md';
import { UserSettingsService } from '../../shared/user-settings.service';
import { Config } from '../../shared/config';
import { HttpClient } from '@angular/common/http';


@Component({
  selector: 'app-user-settings',
  templateUrl: './user-settings.component.html',
  styleUrls: ['./user-settings.component.scss']
})

@Injectable()
export class UserSettingsComponent {
  public versions: VersionDTO[];
  public models: ModelDTO[];
  public selectedVersion: string;
  public selectedModel: string;
  public selectedIsBeta: boolean;

  public errorVersion: string;
  public errorModel: string;

  constructor(http: HttpClient, private userSettingsService: UserSettingsService) {
    http.get<VersionDTO[]>(`${Config.apiUrl}Versions/GetAll`).subscribe(result => {
      this.versions = result;
    });
    http.get<ModelDTO[]>(`${Config.apiUrl}Models/GetAll`).subscribe(result => {
      this.models = result;
    });
    this.selectedVersion = this.userSettingsService.getUserVersion();
    this.selectedModel = this.userSettingsService.getUserModel();
    this.selectedIsBeta = this.userSettingsService.getUserIsBeta();
  }

  @ViewChild(ModalDirective) public basicModal: ModalDirective;

  showModal = () => {
    this.basicModal.show();
  }

  save() {
    const result1 = this.validateVersion(this.selectedVersion);
    const result2 = this.validateModel(this.selectedModel);
    const result = result1 && result2;
    if (result === true) {
      this.userSettingsService.saveUserSettings(this.selectedVersion, this.selectedModel, this.selectedIsBeta);
      this.basicModal.hide();
    }
  }

  validateVersion(version: string): boolean {
    const ver = this.versions.find(x => x.version === version);
    if (ver === undefined) {
      this.errorVersion = 'Selected version is invalid';
      return false;
    } else {
      this.errorVersion = null;
      return true;
    }
  }

  validateModel(model: string): boolean {
    const mod = this.models.find(x => x.name === model);
    if (mod === undefined) {
      this.errorModel = 'Selected model is invalid';
      return false;
    } else {
      this.errorModel = null;
      return true;
    }
  }

  resetVersionError(): void {
    this.errorVersion = null;
  }

  resetModelError(): void {
    this.errorModel = null;
  }
}



interface VersionDTO {
  version: string;
  shortVersion: string;
}

interface ModelDTO {
  name: string;
  arch: string;
  family: string;
  display: string;
}
