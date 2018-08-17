import { Component } from '@angular/core';

@Component({
  selector: 'app-my-map',
  template: `
  <mgl-map
    [style]="'mapbox://styles/mapbox/streets-v9'"
    [zoom]="[9]"
    [center]="[-74.50, 40]"
  >
  </mgl-map>
  `,
  styles: [`
    mgl-map {
      height: 100%;
      width: 100%;
    }
  `]
})
export class DisplayMapComponent { }
