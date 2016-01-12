import { camelize } from 'humps'

export function convertServerErrors(action) {
  const errors = {}, { serverErrors, formInput, messages } = action
  if (serverErrors) {
    for (let field in serverErrors) {
      if (!serverErrors.hasOwnProperty(field) || !serverErrors[field]) continue
      for (let serverError of serverErrors[field]) {
        if (errors[field]) break
        let reason = camelize(serverError.reason)
        let message = 'An unexpected error occurred'
        if (messages[field] && messages[field][reason]) {
          message = formatMessage(messages[field][reason], formInput)
        }
        else if (serverError.message) {
          message = serverError.message
        }
        if (formInput[field]) {
          errors[field] = message
        }
        else if (field === '_error' || !formInput._error) {
          errors._error = message
        }
      }
    }
  }
  return errors
}

function formatMessage(unformattedMessage, formInput) {
  let formattedMessage = '', startIndex, endIndex, token, tokenValue
  if (unformattedMessage.indexOf('{')) {
    for (let i = 0; i < unformattedMessage.length; i++) {
      if (unformattedMessage[i] === '{') {
        if (unformattedMessage[i+1] === '{') {
          formattedMessage += unformattedMessage[i] + unformattedMessage[i+1]
          ++i
          continue
        }
        startIndex = i
        endIndex = unformattedMessage.substr(startIndex).indexOf('}')
        token = unformattedMessage.substr(startIndex + 1, endIndex -1)
        if (formInput && formInput[token] && formInput[token]) {
          tokenValue = formInput[token].trim()
          formattedMessage += tokenValue
          i = startIndex + endIndex
          continue
        }
        else {
          formattedMessage += unformattedMessage[i]
        }
      }
      else {
        formattedMessage += unformattedMessage[i]
      }
    }
  }
  return formattedMessage
}
