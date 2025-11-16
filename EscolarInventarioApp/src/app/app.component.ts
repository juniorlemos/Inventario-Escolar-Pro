import { Component, HostListener, OnInit, Inject, PLATFORM_ID, signal } from '@angular/core';
import { NavigationEnd,Router } from '@angular/router';
import { isPlatformBrowser } from '@angular/common';
import { AuthService } from './core/Services/auth.service';
import { MainComponent } from './layout/main/main.component';
import { LeftSidebarComponent } from './shared/components/left-sidebar/left-sidebar.component';
import { NgxSpinnerModule } from 'ngx-spinner';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { filter } from 'rxjs/operators';



@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, LeftSidebarComponent, MainComponent, NgxSpinnerModule, RouterOutlet],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  isLeftSidebarCollapsed = signal<boolean>(false);
  screenWidth = signal<number>(0);
  isReady = signal<boolean>(false);

  constructor(
    private router: Router,
    private authService: AuthService,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}

  @HostListener('window:resize')
  onResize() {
    if (!isPlatformBrowser(this.platformId)) return;

    const width = window.innerWidth;
    this.screenWidth.set(width);
    if (width < 768) {
      this.isLeftSidebarCollapsed.set(true);
    }
  }

 ngOnInit(): void {
  if (!isPlatformBrowser(this.platformId)) return;

  const width = window.innerWidth;
  const stored = localStorage.getItem('sidebarCollapsed');
  this.screenWidth.set(width);
  this.isLeftSidebarCollapsed.set(
    stored !== null ? stored === 'true' : width < 768
  );

  this.router.events
    .pipe(filter(event => event instanceof NavigationEnd))
    .subscribe(() => {
      if (!this.authService.isLoggedIn() && !this.isLoginRoute()) {
        this.router.navigate(['/login']);
      }
    });

  this.isReady.set(true);
}
  changeIsLeftSidebarCollapsed(isCollapsed: boolean): void {
    if (!isPlatformBrowser(this.platformId)) return;
    this.isLeftSidebarCollapsed.set(isCollapsed);
    localStorage.setItem('sidebarCollapsed', String(isCollapsed));
  }

 isLoginRoute(): boolean {
  const authRoutes = ['/login', '/forgot-password', '/forgot-password/confirmation', '/reset-password'];
  return authRoutes.some(route => this.router.url.startsWith(route));
}
}
