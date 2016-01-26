import standardApi from '../Shared/standardApi'
import { createAction } from 'redux-actions'
import selectCommandRejectionErrors from '../Shared/selectors/commandRejectionErrors'
import { messages } from './validation'
import { routeActions } from 'redux-simple-router'
import _ from 'lodash'

export const LOGIN = {
  SENT: 'LOGIN_SENT',
  FAIL: 'LOGIN_FAIL',
  DONE: 'LOGIN_DONE',
  DATA: 'LOGIN_DATA'
}

export const submitLogin = (formInput, dispatch) => (
  new Promise((resolve, reject) => {
    const { returnUrl, ...body } = formInput
    return dispatch(standardApi.createAction({
      types: [LOGIN.SENT, LOGIN.DONE, LOGIN.FAIL],
      method: 'POST',
      endpoint: `/api/login${returnUrl ? `?returnUrl=${returnUrl}` : ''}`,
      body: body,
    }))
      .then((action) => {
        const errors = selectCommandRejectionErrors(body, action, messages)
        const location = action.type === LOGIN.DONE && _.get(action, 'payload.headers.location', false)
        return errors
          ? reject(errors)
          : location
            ? dispatch(createAction(LOGIN.DATA)(_.get(action, 'payload.data')))
                .then(() => (dispatch(routeActions.push(location))))
            : resolve()
      })
  })
)

export const LOGOFF = {
  SENT: 'LOGOFF_SENT',
  FAIL: 'LOGOFF_FAIL',
  DONE: 'LOGOFF_DONE',
  DATA: 'LOGOFF_DATA',
}

export const submitLogoff = (formInput, dispatch) => (
  new Promise((resolve, reject) => {
    const { returnUrl } = formInput
    return dispatch(standardApi.createAction({
      types: [LOGOFF.SENT, LOGOFF.DONE, LOGOFF.FAIL],
      method: 'POST',
      endpoint: `/api/logoff${returnUrl ? `?returnUrl=${returnUrl}` : ''}`,
    }))
      .then((action) => {
        const location = action.type === LOGOFF.DONE && _.get(action, 'payload.headers.location', false)
        return location
          ? dispatch(createAction(LOGOFF.DATA)())
              .then(() => (dispatch(routeActions.push(location))))
          : resolve()
      })
  })
)
