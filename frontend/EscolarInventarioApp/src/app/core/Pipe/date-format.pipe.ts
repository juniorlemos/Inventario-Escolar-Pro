import { DatePipe } from '@angular/common';
import { Inject, LOCALE_ID, Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'dateFormat'
})
export class DateFormatPipe implements PipeTransform {

    constructor(@Inject(LOCALE_ID) private locale: string) {}

transform(value: string | Date | undefined, format: string = 'dd/MM/yyyy'): string | null {
  if (!value) return null;
  const datePipe = new DatePipe(this.locale);
  return datePipe.transform(value, format);
}
  }



