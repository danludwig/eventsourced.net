export const messages = {
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

export default values => {
  const errors = { }
  if (!values.code) errors.code = messages.code.empty
  return errors
}
