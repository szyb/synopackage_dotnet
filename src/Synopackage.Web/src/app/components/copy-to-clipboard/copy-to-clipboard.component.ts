import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-copy-to-clipboard',
  templateUrl: './copy-to-clipboard.component.html',
  styleUrls: ['./copy-to-clipboard.component.scss'],
})

export class CopyToClipboardComponent {
  @Input() public tooltipSuffix: string;
  @Input() public urlToCopy: string;

  copyToClipboard() {
    navigator.clipboard.writeText(this.urlToCopy);
    var tooltip = document.getElementById("tooltip" + this.tooltipSuffix);
    tooltip.innerHTML = "Copied!";
  }

  resetTooltip() {
    var tooltip = document.getElementById("tooltip" + this.tooltipSuffix);
    tooltip.innerHTML = "Copy to clipboard";
  }
}
