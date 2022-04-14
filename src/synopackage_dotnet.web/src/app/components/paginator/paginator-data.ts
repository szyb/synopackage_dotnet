import { Subject } from 'rxjs';
import { PagingDTO } from 'src/app/shared/model';

export declare interface PaginatorDataProvider {
  /**
   * A callback method that is invoked immediately after
   * Angular has completed initialization of a component's view.
   * It is invoked only once when the view is instantiated.
   *
   */
  dataChanged: Subject<PagingDTO>;
}
