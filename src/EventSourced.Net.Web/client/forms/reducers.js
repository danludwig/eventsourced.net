import { camelize } from 'humps'
import { createSelector } from 'reselect'

const selectState = (state, props) => {
  let formState = state.app.ui
  if (props.parentUi)
    for (let parentUi of props.parentUi)
      formState = formState[parentUi]
  return formState[props.formKey]
}
const selectValues = (state, props) => props.values
const selectServerErrors = createSelector(
  selectState,
  selectValues,
  (state, props) => {
    const errors = {}, { serverErrors, messages } = state
    if (serverErrors) {
      for (let field in serverErrors) {
        if (!serverErrors.hasOwnProperty(field) || !serverErrors[field]) continue
        for (let serverError of serverErrors[field]) {
          if (errors[field]) break
          let reason = camelize(serverError.reason)
          let message = 'An unexpected error occurred'
          if (messages && messages[field] && messages[field][reason]) {
            message = formatMessage(messages[field][reason], props)
          }
          else if (serverError.message) {
            message = serverError.message
          }
          if (props[field]) {
            errors[field] = message
          }
          else if (field === '_error' || !props._error) {
            errors._error = message
          }
        }
      }
    }
    return errors
  }
)

export const selectForm = createSelector(
  [selectState, selectServerErrors],
  (state, serverErrors) => {
    return {
      submitting: state.submitting || false,
      serverErrors
    }
  }
)

const formatMessage = function(template, values) {
  let message = '', startIndex, endIndex, token, tokenValue
  if (template.indexOf('{')) {
    for (let i = 0; i < template.length; i++) {
      if (template[i] === '{') {
        if (template[i+1] === '{') {
          message += template[i] + template[i+1]
          ++i
          continue
        }
        startIndex = i
        endIndex = template.substr(startIndex).indexOf('}')
        token = template.substr(startIndex + 1, endIndex -1)
        if (values && values[token] && values[token]) {
          tokenValue = values[token].trim()
          message += tokenValue
          i = startIndex + endIndex
          continue
        }
        else {
          message += template[i]
        }
      }
      else {
        message += template[i]
      }
    }
  }
  return message
}
