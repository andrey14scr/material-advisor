export function toFullTimeFormat(time: number): string {
  let timeLabel = '';
  if (time) {
    const seconds = time % 60;
    const minutes = Math.floor((time % 3600) / 60);
    const hours = Math.floor(time / 3600);

    if (hours) {
      timeLabel += `${hours}h`;
    }
    if (minutes) {
      if (hours) {
        timeLabel += ' ';
      }
      timeLabel += `${minutes}m`;
    }
    if (seconds) {
      if (hours || minutes) {
        timeLabel += ' ';
      }
      timeLabel += `${seconds}s`;
    }

    return timeLabel;
  }

  return timeLabel;
}