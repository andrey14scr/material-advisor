export function removeEmptyField(obj: any, field: string): any {
  if (!obj[field]) {
    delete obj[field];
  }
  return obj;
}