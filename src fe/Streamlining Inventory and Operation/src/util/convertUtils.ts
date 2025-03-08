export const adjustTimezone = (dateString: string, offset: number) => {
    const date = new Date(dateString);
    date.setHours(date.getHours() + offset);
    return date.toLocaleString();
  };