export const messages = {
  username: {
    empty: 'Username is required.',
    invalidFormat: 'Username can only contain letters, numbers, hyphens, underscores, dots, and must be between 2 and 12 characters long.',
    alreadyExists: 'Sorry, the username **{username}** is already taken. Please choose a different username.',
    phoneNumber: 'You cannot use a phone number as your username.',
    success: 'You will also be able to login with the username above.'
  },
}

export default values => {
  const errors = { }
  if (!values.username) errors.username = messages.username.empty
  return errors
}
