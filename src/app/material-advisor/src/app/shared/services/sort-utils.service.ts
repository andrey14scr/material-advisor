export function sortByStartDate(array: any[], isAsc: boolean = true): any[] {
  if (isAsc) {
    return array.sort((a, b) => new Date(a.startDate).getTime() - new Date(b.startDate).getTime());
  }
  else {
    return array.sort((a, b) => new Date(b.startDate).getTime() - new Date(a.startDate).getTime());
  }
}