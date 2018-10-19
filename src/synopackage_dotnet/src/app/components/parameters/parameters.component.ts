import { Component, Inject, OnInit, OnDestroy, Injectable, ViewChild, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute, ParamMap, Params } from '@angular/router';
import { switchMap, take } from 'rxjs/operators';
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
