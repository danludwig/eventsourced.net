export const messages = {
  emailOrPhone: {
    empty: 'Email address or phone number is required.',
    invalidFormat: '**{emailOrPhone}** does not appear to be a valid email address or US phone number.',
    alreadyExists: '**{emailOrPhone}** has already been registered. [Did you mean to log in](/login)?'
  },
  principal: {
    notLoggedOff: 'You are already logged in as **{username}**. Please log off to register a new user account.'
  }
}

export default values => {
  const errors = { }
  if (!values.emailOrPhone) errors.emailOrPhone = messages.emailOrPhone.empty
  return errors
}
