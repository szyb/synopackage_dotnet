import { Component, ViewChild, Injectable } from '@angular/core';
import { ModalDirective } from 'angular-bootstrap-md';
import { UserSettingsService } from './user-settings.service';
import { Config } from '../../shared/config';
import { HttpClient } from '@angular/common/http';


@Component({
  selector: 'app-user-settings',
  templateUrl: './user-settings.component.html',
})

@Injectable()
export class UserSettingsComponent {
  private versions : VersionDTO[];
  
  constructor(http: HttpClient, private userSettingsService : UserSettingsService) {
        http.get<VersionDTO[]>(`${Config.apiUrl}Versions/GetAll`).subscribe( result => {
          this.versions = result;
        });
  }
  
  @ViewChild(ModalDirective) public basicModal:ModalDirective;

  showModal = () => {
    this.basicModal.show();
  };

  save()
  {
    this.userSettingsService.saveUserSettings("xxx", 'DS718+');
    this.basicModal.hide();
  }
}



interface VersionDTO {
    version: string;
  }
  