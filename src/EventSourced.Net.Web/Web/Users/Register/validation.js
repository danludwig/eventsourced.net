export const registerMessages = {
  emailOrPhone: {
    empty: 'Email address or phone number is required.',
    invalidFormat: '**{emailOrPhone}** does not appear to be a valid email address or US phone number.',
    alreadyExists: '**{emailOrPhone}** has already been registered. [Did you mean to log in](/login)?'
  },
  principal: {
    notLoggedOff: 'You are already logged in as **{username}**. Please log off to register a new user account.'
  }
}

export const validateRegister = values => {
  const errors = { }
  if (!values.emailOrPhone) errors.emailOrPhone = registerMessages.emailOrPhone.empty
  return errors
}

export const verifyMessages = {
  code: {
    empty: 'Secret code is required.',
    unverified: 'Invalid code. You can try {codeAttemptsRemainingCount} more time(s).',
    maxAttempts: 'You have exceeded the maximum allowable secret code attempts.',
    alreadyApplied: 'Your code has already been validated. Please use the forward button on your browser and do not navigate back to this page.'
  },
  correlationId: {
    'null': 'Something went wrong. Please restart the registration process.'
  }
}

export const validateVerify = values => {
  const errors = { }
  if (!values.code) errors.code = verifyMessages.code.empty
  return errors
}

export const redeemMessages = {
  emailOrPhone: {
    alreadyExists: 'The login **{attemptedValue}** has already been registered. Did you forget your password?'
  },
  username: {
    empty: 'Username is required.',
    invalidFormat: 'Username can only contain letters, numbers, hyphens, underscores, dots, and must be between 2 and 12 characters long.',
    alreadyExists: 'Sorry, the username **{username}** is already taken. Please choose a different username.',
    phoneNumber: 'You cannot use a phone number as your username.',
    success: 'You will also be able to login with the username above.'
  },
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

export const validateRedeem = values => {
  const errors = { }
  if (!values.username) errors.username = redeemMessages.username.empty
  if (!values.password) errors.password = redeemMessages.password.empty
  if (!values.passwordConfirmation) errors.passwordConfirmation = redeemMessages.passwordConfirmation.empty
  return errors
}
