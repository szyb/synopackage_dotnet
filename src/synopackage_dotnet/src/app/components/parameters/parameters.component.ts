import { Component, OnInit, OnDestroy, Injectable, Input } from '@angular/core';
import { ParametersDTO } from '../../shared/model';

@Component({
  selector: 'app-parameters',
  templateUrl: './parameters.component.html',
})
@Injectable()
export class ParametersComponent implements OnInit, OnDestroy {

  @Input()
  public parameters: ParametersDTO;

  @Input()
  public url: string;

  @Input()
  public www: string;

  @Input()
  public showSourceName: boolean;

  constructor() {
    this.showSourceName = true;
  }

  public getChannel(isBeta: boolean): string {
    if (isBeta) {
      return 'Beta';
    } else {
      return 'Stable';
    }
  }


  ngOnInit() {

  }
  ngOnDestroy(): void {
  }
}
