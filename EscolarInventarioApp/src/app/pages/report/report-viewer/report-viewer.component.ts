import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SafeResourceUrl, DomSanitizer } from '@angular/platform-browser';


@Component({
  selector: 'app-report-viewer',
  imports: [CommonModule],
  templateUrl: './report-viewer.component.html',
  styleUrl: './report-viewer.component.scss'
})
export class ReportViewerComponent {
  @Input() pdfUrl: string | null = null;
  safeUrl: SafeResourceUrl | null = null;
  constructor(private sanitizer: DomSanitizer) {}

  ngOnChanges(): void {
    if (this.pdfUrl) {
      this.safeUrl = this.sanitizer.bypassSecurityTrustResourceUrl(this.pdfUrl);
    }
  }
}
