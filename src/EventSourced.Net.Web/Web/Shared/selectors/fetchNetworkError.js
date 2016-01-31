import { camelize } from 'humps'
import format from 'string-template'
import { messages as errorMessages } from '../../Errors/validation'

export default action => {
  const errors = {}, { error, payload } = action
  if (error === true && payload.status !== 400) {
    let message = errorMessages.unknown
    if (payload && payload.statusText) {
      const template = errorMessages.api[camelize(payload.statusText)]
      if (template) {
        message = errorMessages.unexpected + ' ' + template
      }
    }
    else if (payload && payload.name && payload.message) {
      const template = errorMessages.api[camelize(payload.name)]
      if (template) {
        message = format(template, payload)
        const camelizedMessage = camelize(payload.message)
        if (errorMessages.api[camelizedMessage])
          message = errorMessages.unexpected + ' ' + errorMessages.api[camelizedMessage]
      }
    }
    errors._error = message
  }
  return errors
}
