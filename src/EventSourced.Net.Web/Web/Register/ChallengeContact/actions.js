import standardApi from '../../Shared/actions/standardApi'
import { createAction } from 'redux-actions'
import { routeActions } from 'redux-simple-router'
import _ from 'lodash'
import selectCommandRejectionErrors from '../../Shared/selectors/commandRejectionErrors'
import { messages } from './validation'

export const REGISTER = {
  SENT: 'REGISTER_SENT',
  FAIL: 'REGISTER_FAIL',
  DONE: 'REGISTER_DONE',
}

export default (formInput, dispatch) => (
  new Promise((resolve, reject) => (
    dispatch(standardApi({
      types: [REGISTER.SENT, REGISTER.DONE, REGISTER.FAIL],
      method: 'POST',
      endpoint: `/api/register`,
      body: formInput,
    }))
      .then((action) => {
        const errors = selectCommandRejectionErrors(formInput, action, messages)
        const location = action.type === REGISTER.DONE && _.get(action, 'payload.headers.location', false)
        return errors ?
          reject(errors) :
          location ?
            dispatch(routeActions.push(location)) :
            resolve()
      })
  ))
)
