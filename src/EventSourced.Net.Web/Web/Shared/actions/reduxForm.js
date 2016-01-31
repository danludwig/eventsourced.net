import * as reduxActions from 'redux-form'

export const startAsyncValidation = (form, field, key) => {
  let action = reduxActions.startAsyncValidation(form, field)
  if (key) action = { ...action, key, }
  return action
}


export const stopAsyncValidation = (form, errors, key) => {
  let action = reduxActions.stopAsyncValidation(form, errors)
  if (key) action = { ...action, key, }
  return action
}

export const stopSubmit = (form, errors, key) => {
  let action = reduxActions.stopSubmit(form, errors)
  if (key) action = { ...action, key, }
  return action
}

export const blur = (form, field, value, touch, key) => {
  let action = reduxActions.blur(form, field, value)
  if (touch) action = { ...action, touch }
  if (key) action = { ...action, key }
  return action
}
