import { Injectable, Component } from '@angular/core';
import { UserSettingsService } from '../../shared/user-settings.service';


@Component({
  selector: 'app-user-settings-display',
  templateUrl: './user-settings-display.component.html'
})

@Injectable()
export class UserSettingsDisplayComponent {
  private selectedVersion: string;
  private selectedModel: string;
  private selectedIsBeta: boolean;
  private stableOrBeta: string;

  constructor(private userSettingsService: UserSettingsService) {
    this.userSettingsService.events.subscribe(
      () => {
        this.init();
      }
    );
    this.init();
  }

  init() {
    this.selectedVersion = this.userSettingsService.getUserVersion();
    this.selectedModel = this.userSettingsService.getUserModel();
    this.selectedIsBeta = this.userSettingsService.getUserIsBeta();
    if (this.selectedIsBeta) {
      this.stableOrBeta = 'Beta';
    } else {
      this.stableOrBeta = 'Stable';
    }
  }

  isSetup(): boolean {
    return this.userSettingsService.isSetup();
  }

}
