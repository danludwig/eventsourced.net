import { handleActions } from 'redux-actions'
import standardApi from '../../Shared/reducers/standardApi'
import { INITIALIZE_STATE } from '../../Shared/actions/initializeState'
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
  [REDEEM.SENT]: standardApi.sent,
  [REDEEM.FAIL]: standardApi.fail,
  [REDEEM.DONE]: standardApi.done,
  [REDEEM.OVER]: standardApi.over,
}, standardApi.initialState)
