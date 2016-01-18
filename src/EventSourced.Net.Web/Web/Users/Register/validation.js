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

export const registerValidate = values => {
  const errors = { }
  if (!values.emailOrPhone) errors.emailOrPhone = registerMessages.emailOrPhone.empty
  return errors
}

export const verifyMessages = {
  code: {
    empty: 'Secret code is required.',
    unverified: 'Invalid code. You can try {codeAttemptsRemainingCount} more time(s).',
    maxAttempts: 'You have exceeded the maximum allowable secret code attempts.',
    stateConflict: 'Your code has already been validated. Please use the forward button on your browser and do not navigate back to this page.'
  },
  correlationId: {
    'null': 'Something went wrong. Please restart the registration process.'
  }
}

export const verifyValidate = values => {
  const errors = { }
  if (!values.code) errors.code = verifyMessages.code.empty
  return errors
}
