import { Component } from '@angular/core';
import { ReportService } from '../../../core/Services/report.service';
import { ReportViewerComponent } from '../report-viewer/report-viewer.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-report-list',
  imports: [ReportViewerComponent,CommonModule],
  templateUrl: './report-list.component.html',
  styleUrl: './report-list.component.scss'
})
export class ReportListComponent {
 pdfUrl: string | null = null;
 selectedReport: string | null = null;

  constructor(private reportService: ReportService) {}

  openPdf(blob: Blob) {
    const url = URL.createObjectURL(blob);
    this.pdfUrl = url;
  }

  downloadPdf(blob: Blob, fileName: string) {
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = fileName;
    a.click();
    URL.revokeObjectURL(url);
  }

   generateReport(reportName: string) {
    this.selectedReport = reportName;

    switch (reportName) {
      case 'inventory':
        this.reportService.getInventoryReport().subscribe(blob => this.openPdf(blob));
        break;
      case 'by-location':
        this.reportService.getAssetsByLocationReport().subscribe(blob => this.openPdf(blob));
        break;
      case 'movements':
        this.reportService.getAssetMovementsReport().subscribe(blob => this.openPdf(blob));
        break;
      case 'canceled-movements':
        this.reportService.getAssetCanceledMovementsReport().subscribe(blob => this.openPdf(blob));
        break;
      case 'conservation':
        this.reportService.getAssetsByConservationReport().subscribe(blob => this.openPdf(blob));
        break;
      case 'by-category':
        this.reportService.getAssetsByCategoryReport().subscribe(blob => this.openPdf(blob));
        break;
    }
  }
}

