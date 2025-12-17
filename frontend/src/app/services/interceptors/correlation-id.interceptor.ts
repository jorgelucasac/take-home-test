import { HttpInterceptorFn } from '@angular/common/http';

const correlationHeader = 'X-Correlation-Id';

const generateCorrelationId = (): string => {
    return crypto.randomUUID();
};

export const correlationIdInterceptor: HttpInterceptorFn = (req, next) => {
  if (!req.headers.has(correlationHeader)) {
    req = req.clone({
      setHeaders: {
        [correlationHeader]: generateCorrelationId(),
      },
    });
  }

  return next(req);
};