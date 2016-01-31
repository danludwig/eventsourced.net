import { createSelector } from 'reselect'
import { camelize } from 'humps'
import format from 'string-template'

export default (formInput, action, messages) => {
  if (!action || !action.error || !action.payload || action.payload.status !== 400) return undefined
  const { response } = action.payload
  const _error = "An unexpected error occurred."
  if (!response) return { _error }

  const errors = {}
  for (const field in response) {
    if (!response.hasOwnProperty(field) || !response[field]) continue
    for (const commandRejection of response[field]) {
      if (errors[field]) break // only use first command rejection
      const reason = camelize(commandRejection.reason)
      let message = _error
      if (messages && messages[field] && messages[field][reason]) {
        const tokens = { ...formInput, ...commandRejection.data, }
        if (!tokens[field] && commandRejection.value)
          tokens[field] = commandRejection.value
        message = format(messages[field][reason], tokens)
      }
      else if (commandRejection.message) {
        message = commandRejection.message
      }
      if (formInput && formInput.hasOwnProperty(field)) {
        errors[field] = message
      }
      else if (field === '_error' || !errors._error) {
        errors._error = message
      }
    }
  }
  return errors
}
