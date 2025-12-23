import { HttpInterceptorFn } from '@angular/common/http';

const correlationHeader = 'X-Correlation-Id';
const appNameHeader = 'X-Application-Name';
const applicationName = 'AngularFrontApp';

const generateCorrelationId = (): string => {
    return crypto.randomUUID();
};

export const correlationInterceptor: HttpInterceptorFn = (req, next) => {
  if (!req.headers.has(correlationHeader)) {
    req = req.clone({
      setHeaders: {
        [correlationHeader]: generateCorrelationId(),
        [appNameHeader]: applicationName,
      },
    });
  }

  return next(req);
};