import { Injectable, Component, OnChanges, Input } from '@angular/core';
import { PackageDTO } from 'src/app/sources/sources.model';


@Component({
  selector: 'app-package-info',
  templateUrl: './package-info.component.html'
})

@Injectable()
export class PackageInfoComponent {

  @Input()
  public package: PackageDTO;

  constructor() {
  }

  init() {
  }


}
