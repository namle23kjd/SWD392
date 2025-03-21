export const LAYOUT_SHELF = {
  labelCol: { span: 8 },
  wrapperCol: { span: 16 },
};
export const LAYOUT_LOT = {
    labelCol: { span: 8 },
    wrapperCol: { span: 16 },
  };
  // Simplified validation messages
export const VALIDATE_MESSAGES = {
  required: '${label} is required',
  types: {
    email: 'Invalid email format',
    number: 'Invalid number format',
    date: 'Invalid date format'
  },
  number: {
    range: '${label} must be between ${min} and ${max}'
  }
};

// Custom date validator functions for Ant Design DatePicker (using dayjs objects)
export const validateManufactureDate = () => ({
  validator(_ :any, value:any) {
    if (!value) {
      return Promise.resolve();
    }
    
    if (value.isBefore(new Date())) {
      return Promise.reject(new Error('Manufacture date must be in the future'));
    }
    return Promise.resolve();
  }
});

export const validateExpiryDate = (getFieldValue: any) => ({
  validator(_: any, value: any) {
    if (!value) {
      return Promise.resolve();
    }
    
    // Check if expiry date is in the future
    const currentDate = new Date();
    if (value.valueOf() < currentDate.valueOf()) {
      return Promise.reject(new Error('Expiry date must be in the future'));
    }
    
    // Get manufacture date from form
    const manufactureDate = getFieldValue(['manufactureDate']);
    if (!manufactureDate) {
      return Promise.resolve();
    }
    console.log('value', value);
    console.log('manufactureDate', manufactureDate);
    // Compare dates directly using valueOf() for timestamp comparison
    if (value.valueOf() <= manufactureDate.valueOf()) {
      return Promise.reject(new Error('Expiry date must be after manufacture date'));
    }
    
    return Promise.resolve();
  }
});