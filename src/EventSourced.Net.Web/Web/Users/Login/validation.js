export const messages = {
  login: {
    empty: 'Email or phone is required.',
    unverified: 'Invalid login or password.'
  },
  password: {
    empty: 'Password is required.'
  }
}

export default values => {
  const errors = { }
  if (!values.login) errors.login = messages.login.empty
  if (!values.password) errors.password = messages.password.empty
  return errors;
}
