import { createSelector } from 'reselect'
import { camelize } from 'humps'
import format from 'string-template'

export default (formInput, action, messages) => {
  if (!action || !action.payload || !action.payload.errors) return undefined
  const { errors } = action.payload
  const _error = "An unexpected error occurred."
  if (!errors) return { _error }

  const formErrors = {}
  for (const field in errors) {
    if (!errors.hasOwnProperty(field) || !errors[field] || !errors[field].reasonText) continue
    const reason = camelize(errors[field].reasonText)
    const tokens = { ...formInput }
    if (!tokens[field]) tokens[field] = errors[field].value
    let message = _error
    if (messages && messages[field] && messages[field][reason]) {
      message = format(messages[field][reason], tokens)
    }
    if (formInput && formInput.hasOwnProperty(field)) {
      formErrors[field] = message
    }
    else if (!formErrors._error) {
      formErrors._error = message
    }
  }
  return formErrors
}
