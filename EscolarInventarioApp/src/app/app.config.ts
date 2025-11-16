import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { HTTP_INTERCEPTORS, provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';

import { provideToastr } from 'ngx-toastr';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { authInterceptor } from '../interceptors/auth.interceptor';
import { loadingInterceptor } from '../interceptors/loading.interceptor';

import { provideEnvironmentNgxMask } from 'ngx-mask';

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(withFetch(),
      withInterceptors([authInterceptor, loadingInterceptor])
    ),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideAnimations(),
    BsModalService,
    provideToastr({
      timeOut: 4000,
      preventDuplicates: true,
      progressBar: true
    }),
    provideEnvironmentNgxMask()
  ]
};
