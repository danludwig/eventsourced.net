import validateUsername, { messages as username } from '../ValidateUsername/validation'

export const messages = {
  emailOrPhone: {
    alreadyExists: 'The login **{emailOrPhone}** has already been registered. Did you forget your password?'
  },
  ...username,
  password: {
    empty: 'Password is required.',
    notEqual: 'Passwords do not match.',
    invalidFormat: 'Password must contain at least {minCharacters} characters.'
  },
  passwordConfirmation: {
    empty: 'Password confirmation is required.'
  },
  token: {
    stateConflict: 'Your password has already been created. Please use the forward button on your browser and do not navigate back to this page.',
    unverified: 'Your opportunity to complete registration has expired. Please restart the registration process.'
  },
  correlationId: {
    'null': 'Something went wrong. Please restart the registration process.'
  }
}

export default values => {
  const errors = validateUsername(values)
  if (!values.password) errors.password = messages.password.empty
  if (!values.passwordConfirmation) errors.passwordConfirmation = messages.passwordConfirmation.empty
  return errors
}
