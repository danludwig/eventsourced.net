import standardApi from '../../Shared/standardApi'
import { createAction } from 'redux-actions'
import { routeActions } from 'redux-simple-router'
import _ from 'lodash'
import selectCommandRejectionErrors from '../../Shared/selectors/commandRejectionErrors'
import { messages } from './validation'

export const VERIFY = {
  SENT: 'REGISTER/VERIFY_SENT',
  FAIL: 'REGISTER/VERIFY_FAIL',
  DONE: 'REGISTER/VERIFY_DONE',
  DATA: 'REGISTER/VERIFY_DATA',
}

export default (formInput, dispatch) => (
  new Promise((resolve, reject) => {
    const { correlationId, ...body } = formInput
    return dispatch(standardApi.createAction({
      types: [VERIFY.SENT, VERIFY.DONE, VERIFY.FAIL],
      method: 'POST',
      endpoint: `/api/register/${correlationId}`,
      body: body,
    }))
      .then(action => {
        const errors = selectCommandRejectionErrors(body, action, messages)
        const location = action.type === VERIFY.DONE && _.get(action, 'payload.headers.location', false)
        return errors ?
          reject(errors) :
          location ?
            dispatch(createAction(VERIFY.DATA)(_.get(action, 'payload.data')))
                .then(() => (dispatch(routeActions.push(location)))) :
            resolve()
      })
  })
)
