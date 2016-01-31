import { createSelector } from 'reselect'
import { camelize } from 'humps'
import format from 'string-template'
import _ from 'lodash'

export default (formInput, action, messages) => {
  if (!action || !action.error || !action.payload || action.payload.status !== 400) return undefined
  const { response } = action.payload
  const _error = "An unexpected error occurred."
  if (!response) return { _error }


  const { reasonInvalid } = response
  const key = camelize(reasonInvalid)

  if (messages && messages[key])
    return format(messages[key], formInput)
  return _error
}
