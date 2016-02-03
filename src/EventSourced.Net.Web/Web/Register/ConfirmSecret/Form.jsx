import { reduxForm } from 'redux-form'
import validate from './validation'
import ValidationSummary from '../../Shared/ValidationSummary'
import ReduxFormPropTypes from '../../Shared/propTypes/reduxForm'

const form = 'verify'
const fields = ['code', 'correlationId', 'returnUrl']
const config = { form, fields, validate, }

const Form = ({ handleSubmit, submitting, submitFailed, error, errors, fields: { code }, }) => (
  <form method="post" className="form-horizontal" role="form" onSubmit={handleSubmit}>
    <div className="form-group">
      <div className="col-md-3">
        <label className="control-label sr-only">Secret code</label>
        <input type="text" name="code" className="form-control" placeholder="Secret code" disabled={submitting} {...code} />
      </div>
    </div>
    <div className="form-group">
      <div className="col-md-10">
        <button type="submit" className="btn btn-default" disabled={submitting}>Verify</button>
      </div>
    </div>
    <ValidationSummary errors={{ error, ...errors }} visible={submitFailed && !submitting} />
  </form>
)

Form.propTypes = {
  ...ReduxFormPropTypes.handleSubmit,
  ...ReduxFormPropTypes.submitting,
  ...ReduxFormPropTypes.submitFailed,
  ...ReduxFormPropTypes.error,
  ...ReduxFormPropTypes.formName,
  errors: React.PropTypes.shape({
    code: React.PropTypes.string,
  }).isRequired,
  fields: React.PropTypes.shape({
    code: ReduxFormPropTypes.field.isRequired,
    correlationId: ReduxFormPropTypes.field.isRequired,
    returnUrl: ReduxFormPropTypes.field.isRequired,
  }).isRequired,
}

const select = () => ({ formName: form, })
export default reduxForm(config, select)(Form)
