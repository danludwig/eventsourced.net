import standardApi from '../../Shared/actions/standardApi'
import selectReasonInvalidError from '../../Shared/selectors/reasonInvalidError'
import { messages } from './validation'

export const VALIDATE_USERNAME = {
  SENT: 'VALIDATE_USERNAME_SENT',
  FAIL: 'VALIDATE_USERNAME_FAIL',
  DONE: 'VALIDATE_USERNAME_DONE',
}

export default (formInput, dispatch) => {
  return new Promise((resolve, reject) => {
    const { username, ...rest } = formInput
    return dispatch(standardApi({
      types: [VALIDATE_USERNAME.SENT, VALIDATE_USERNAME.DONE, VALIDATE_USERNAME.FAIL],
      method: 'POST',
      endpoint: `/api/validate-username`,
      body: { username, },
    }))
      .then(action => {
        const error = selectReasonInvalidError({ username }, action, messages.username)
        return error ?
          reject({ username: error }) :
          resolve()
      })
  })
}
