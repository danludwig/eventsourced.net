import standardApi from '../Shared/standardApi'
import { createAction } from 'redux-actions'
import { routeActions } from 'redux-simple-router'
import _ from 'lodash'

export const LOGOFF = {
  SENT: 'LOGOFF_SENT',
  FAIL: 'LOGOFF_FAIL',
  DONE: 'LOGOFF_DONE',
  DATA: 'LOGOFF_DATA',
}

export default (formInput, dispatch) => (
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
