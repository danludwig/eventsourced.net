import { camelize } from 'humps'
import format from 'string-template'
import { messages as errorMessages } from '../../Errors/validation'
import fetchNetworkError from './fetchNetworkError'
import _ from 'lodash'

export default (formInput, action, messages) => {
  const errors = fetchNetworkError(action)
  if (errors._error) return errors._error
  if (!action || !action.error || !action.payload || action.payload.status !== 400) return undefined
  const { response } = action.payload
  const _error = errorMessages.unexpected
  if (!response) return { _error }

  const { reasonInvalid } = response
  const key = camelize(reasonInvalid)

  if (messages && messages[key])
    return format(messages[key], formInput)
  return _error
}
