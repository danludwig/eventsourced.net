import { handleActions } from 'redux-actions'
import standardApi from '../../Shared/standardApi'
import { INITIALIZE_STATE } from '../../Shared/actions'
import { VERIFY } from '../ConfirmSecret/actions'
import { REDEEM } from './actions'
import _ from 'lodash'

export default handleActions({
  [INITIALIZE_STATE]: (state, action) =>
    Object.assign({}, state, {
      viewData: _.get(action, 'payload.app.register.createLogin.viewData', {}),
    }
  ),
  [VERIFY.DATA]: (state, action) =>
    Object.assign({}, state, {
      viewData: action.payload,
    }
  ),
  [REDEEM.SENT]: standardApi.reducers.sent,
  [REDEEM.FAIL]: standardApi.reducers.fail,
  [REDEEM.DONE]: standardApi.reducers.done,
  [REDEEM.OVER]: (state, action) =>
    Object.assign({}, state, {
      apiCalls: [
        Object.assign({}, state.apiCalls[0], {
          over: { ...action.payload, },
        }),
        ...state.apiCalls.slice(1)
      ],
    }
  ),
}, standardApi.initialState)
