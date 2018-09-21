import { Component, ViewChild } from '@angular/core';
import { ModalDirective } from 'angular-bootstrap-md';


@Component({
  selector: 'app-dialog',
  templateUrl: './dialog.component.html'
})

export class DialogComponent {
  public message: string;
  @ViewChild(ModalDirective) public dialogModal: ModalDirective;

  constructor() {
  }

  public setError(error: any) {
    this.message = error;
  }
}
